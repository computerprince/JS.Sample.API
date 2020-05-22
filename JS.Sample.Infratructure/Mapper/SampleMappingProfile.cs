using AutoMapper;
using JS.Sample.Application.ViewModel.Product;
using JS.Sample.Domain;

namespace JS.Sample.Infratructure.Mapper
{
   
    public class SampleMappingProfile : Profile
    {
        public SampleMappingProfile()
        {
            #region Product
            CreateMap<Product, ProductViewModel>();
            #endregion


        }

    }
}
