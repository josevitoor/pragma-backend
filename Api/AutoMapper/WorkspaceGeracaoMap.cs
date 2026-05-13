using AutoMapper;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;

namespace Application.AutoMapper;
/// <summary>
/// Classe para o mapeamento de Workspace Geracao
/// </summary>
public class WorkspaceGeracaoMap : Profile
{
    /// <summary>
    /// Construtor do mapeamento de Workspace Geracao
    /// </summary>
    public WorkspaceGeracaoMap()
    {
        CreateMap<WorkspaceGeracaoPost, WorkspaceGeracao>().ReverseMap();
        CreateMap<WorkspaceGeracaoGet, WorkspaceGeracao>().ReverseMap();
    }
}