using AutoMapper.Internal;
using AutoMapper;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Mappers.AutoMapping
{
    public class AutoMapperProfile : IAutoMapper
    {
        public static List<TypePair> typePairs = new();
        //Đây là một danh sách tĩnh chứa các cặp loại (TypePair), đại diện cho các loại dữ liệu nguồn và đích sẽ được ánh xạ với nhau

        public IMapper _mapper;

        public TDestination Map<TDestination, TSource>(TSource soure, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return _mapper.Map<TSource, TDestination>(soure);
        }

        public IList<TDestination> Map<TDestination, TSource>(IList<TSource> sources, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return _mapper.Map<IList<TSource>, IList<TDestination>>(sources);
        }

        public TDestination Map<TDestination>(object soure, string? ignore = null)
        {
            Config<TDestination, object>(5, ignore);
            return _mapper.Map<TDestination>(soure);


        }

        public IList<TDestination> Map<TDestination>(IList<object> sources, string? ignore = null)
        {
            Config<TDestination, IList<object>>(5, ignore);
            return _mapper.Map<IList<TDestination>>(sources);
        }
        protected void Config<TDestionation, TSource>(int depth = 5, string? ignore = null)
        //config cấu hình khi ánh xạ trong đó độ sau ánh xạ tối đa là 5
        {
            var typePair = new TypePair(typeof(TSource), typeof(TDestionation));
            //Tạo một đối tượng TypePair đại diện cho loại dữ liệu nguồn và đích.
            if (typePairs.Any(a => a.DestinationType == typePair.DestinationType && a.SourceType == typePair.SourceType) && ignore is null)
                return;
            //Kiểm tra xem cặp loại này đã tồn tại trong danh sách typePairs hay chưa. Nếu đã tồn tại và ignore là null, phương thức sẽ dừng và không làm gì thêm

            typePairs.Add(typePair);
            //Nếu cặp loại chưa tồn tại, thêm nó vào danh sách typePairs.


            var config = new MapperConfiguration(cfg =>
            {//Tạo một cấu hình mapper mới, duyệt qua từng cặp loại trong danh sách typePairs và tạo ánh xạ giữa các loại này
                foreach (var item in typePairs)
                {
                    if (ignore is not null)
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(depth).ForMember(ignore, x => x.Ignore()).ReverseMap();
                    else
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(depth).ReverseMap();
                }
            });

            _mapper = config.CreateMapper();//Tạo mapper từ cấu hình vừa được tạo và lưu trữ nó trong biến _mapper
        }
    }
}