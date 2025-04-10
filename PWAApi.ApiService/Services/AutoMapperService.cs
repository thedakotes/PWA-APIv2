using API.DataTransferObjects;
using API.Models;
using AutoMapper;
using PWAApi.ApiService.DataTransferObjects.PlantID;
using PWAApi.ApiService.Models.PlantID;
using PWAApi.ApiService.Models.PlantID.PermaPeople;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        //#region Events
        CreateMap<Event, EventDTO>();
        CreateMap<EventDTO, Event>();
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

        //#region Plant Info
        CreateMap<PWAApi.ApiService.Models.PlantID.Perenual.Plant, PlantDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CommonName));
        CreateMap<Plant, PlantDTO>()
            .ForMember(dest => dest.Edibility, opt => opt.MapFrom(src => MapEdibilityDTO(src)))
            .ForMember(dest => dest.Growth, opt => opt.MapFrom(src => MapPlantDataItem(src.Data, "Growth")))
            .ForMember(dest => dest.WaterRequirement, opt => opt.MapFrom(src => MapPlantDataItem(src.Data, "Water requirement")))
            .ForMember(dest => dest.LightRequirement, opt => opt.MapFrom(src => MapPlantDataItem(src.Data, "Light requirement")))
            .ForMember(dest => dest.USDAHardinessZone, opt => opt.MapFrom(src => MapPlantDataItem(src.Data, "USDA Hardiness zone")))
            .ForMember(dest => dest.Layer, opt => opt.MapFrom(src => MapPlantDataItem(src.Data, "Layer")))
            .ForMember(dest => dest.SoilType, opt => opt.MapFrom(src => MapPlantDataItem(src.Data, "Soil type")));
        //#endregion
    }

    private static EdibilityDTO MapEdibilityDTO(Plant src)
    {
        var isEdible = src.Data.FirstOrDefault(x => x.Key == "Edible")?.Value == "true";
        var parts = src.Data.Where(x => x.Key == "Edible parts").SelectMany(x => x.Value.Split(","));
        return new EdibilityDTO(isEdible, parts);
    }

    private static string MapPlantDataItem(IEnumerable<PlantDataItem> src, string key)
    {
        return src.FirstOrDefault(x => x.Key == key)?.Value ?? string.Empty;
    }
}