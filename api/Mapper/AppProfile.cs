// namespace Api.Mapper;
using Api.Models;
using AutoMapper;

public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<AppState, AppStateDto>();
    }
}
