
using AutoMapper;
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.ModelView;
using DocumentHelper.ViewModel.References.Units;

namespace DocumentHelper.Services.Mapping.Profiles
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<Position, PositionViewModel>()
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                    .ForMember(dest => dest.Item, opt => opt.MapFrom(src => new ItemViewModel
                    {
                        Id = src.ItemId,
                        Name = src.Item.Name,
                        Unit = src.Item.Unit
                    }))
                    .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => new UnitViewModel
                    {
                        Id = src.Item.UnitId, 
                        FullName = src.Item.Unit.FullName,
                        ShortName = src.Item.Unit.ShortName
                    }));

        }
    }
}
