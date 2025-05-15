using API.DataTransferObjects;
using API.Models;
using AutoMapper;
using PWAApi.ApiService.Authentication.DataTransferObjects;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.DataTransferObjects;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models;
using PWAApi.ApiService.Models.PlantID.PlantNet;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        //#region Events
        CreateMap<Event, EventDTO>();
        CreateMap<EventDTO, Event>();
        CreateMap<Reminder, ReminderDTO>();
        CreateMap<ReminderDTO, Reminder>();
        //#endregion

        //#region PlantID
        CreateMap<PlantNetResult, PlantIDDTO>()
            .ForMember(dest => dest.GBIF_ID, opt => opt.MapFrom(src => src.GBIF != null ? src.GBIF.ID : string.Empty))
            .ForMember(dest => dest.POWO_ID, opt => opt.MapFrom(src => src.POWO != null ? src.POWO.ID : string.Empty));
        CreateMap<Model4, PlantIDSpeciesDTO>();
        CreateMap<PlantNetImage, PlantIDImageDTO>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Date != null ? (src.Date.String != null ? src.Date.String : string.Empty) : string.Empty)))
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url != null ? (src.Url.O ?? ( src.Url.M ?? src.Url.S)) : string.Empty));
        CreateMap<TaxonomicRank, TaxonomicRankDTO>();
        CreateMap<IUCN, IUCNDTO>();
        //#endregion

        //#region User
        CreateMap<ApplicationUser, UserDTO>();
        CreateMap<UserDTO, ApplicationUser>();
        //#endregion
    }
}