using AutoMapper;
using CollectionTrackerAPI.Models;
using CollectionTrackerAPI.ViewModels;

namespace CollectionTrackerAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Collection, CollectionViewModel>().ReverseMap();
            CreateMap<Condition, ConditionViewModel>().ReverseMap();
        }
    }
}
