using AutoMapper;
using EFPractica01.DTOs;
using EFPractica01.Models;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using System.Diagnostics;
using System.Text.Json;

namespace EFPractica01.Services
{
    public class ServicesBackground : IHostedService, IDisposable
    {
        private readonly ILogger<ServicesBackground> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer = new Timer(_ => { });
        private IMapper _mapper;

        public ServicesBackground(IHttpClientFactory httpClientFactory,
                                  IServiceScopeFactory scopeFactory,
                                  IMapper mapper,
                                  ILogger<ServicesBackground> logger)
        {
            _httpClientFactory = httpClientFactory;
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            int initialExecutionDelay = 5000; // 5 segundos para la primera ejecución
            int updateInterval = 1800; // Intervalo de 30 minutos

            _timer = new Timer(ApiGetHttp, null,
                               initialExecutionDelay,
                               updateInterval * 1000);

            _logger.LogInformation("Servicio background iniciado.");

            return Task.CompletedTask;
        }

        public async void ApiGetHttp(object? state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _skillcontext = scope.ServiceProvider.GetRequiredService<SkillsContext>();
                var _httpClient = _httpClientFactory.CreateClient(nameof(ServicesBackground));

                _logger.LogInformation("Iniciando solicitud HTTP a la API.");

                var stopwatch = Stopwatch.StartNew();

                try
                {
                    var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    List<SkillsDTOs>? skillsDtos;

                    try
                    {
                        skillsDtos = JsonSerializer.Deserialize<List<SkillsDTOs>>(json, options);
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "Error deserializando la respuesta JSON.");
                        return;
                    }

                    if (skillsDtos == null || skillsDtos.Count == 0)
                    {
                        _logger.LogWarning("La respuesta JSON no contiene datos o la lista de habilidades está vacía.");
                        return;
                    }

                    foreach (var skilldtos in skillsDtos)
                    {
                        try
                        {
                            var verfSkilldtos = await _skillcontext.Skills
                                .Include(s => s.Ranks)
                                .FirstOrDefaultAsync(s => s.Id == skilldtos.Id);

                            if (verfSkilldtos == null)
                            {
                                var newSkill = _mapper.Map<Skills>(skilldtos);
                                _skillcontext.Skills.Add(newSkill);
                                await _skillcontext.SaveChangesAsync();

                                foreach (var rank in skilldtos.Ranks)
                                {
                                    var newRank = _mapper.Map<Ranks>(rank);
                                    newRank.SkillsId = newSkill.Id;
                                    _skillcontext.Ranks.Add(newRank);
                                }
                                await _skillcontext.SaveChangesAsync();
                            }
                            else
                            {
                                var existingRanks = await _skillcontext.Ranks
                                    .Where(r => r.SkillsId == verfSkilldtos.Id)
                                    .ToListAsync();

                                foreach (var rankDto in skilldtos.Ranks)
                                {
                                    var verfRanks = existingRanks.FirstOrDefault(r => r.Level == rankDto.Level);

                                    if (verfRanks == null)
                                    {
                                        var newRank = _mapper.Map<Ranks>(rankDto);
                                        newRank.SkillsId = verfSkilldtos.Id;
                                        _skillcontext.Ranks.Add(newRank);
                                    }
                                }

                                await _skillcontext.SaveChangesAsync();
                            }
                        }
                        catch (DbUpdateException dbEx)
                        {
                            _logger.LogError(dbEx, $"Error de base de datos al actualizar la habilidad '{skilldtos.Name}'.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error inesperado al procesar la habilidad '{skilldtos.Name}'.");
                        }
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    _logger.LogError(httpEx, "Error en la solicitud HTTP a la API.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error inesperado durante la ejecución del método ApiGetHttp.");
                }
                finally
                {
                    stopwatch.Stop();
                    _logger.LogInformation($"El método ApiGetHttp tomó {stopwatch.ElapsedMilliseconds} ms en completarse.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Servicio background detenido.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}