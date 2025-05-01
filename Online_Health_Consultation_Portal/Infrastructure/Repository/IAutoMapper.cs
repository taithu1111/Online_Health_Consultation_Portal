namespace Online_Health_Consultation_Portal.Infrastructure.Repository
{
    public interface IAutoMapper
    {
        //TSource là kiểu nhận được 
        //TDestination là kiểu chuyển về
        TDestination Map<TDestination, TSource>(TSource soure, string? ignore = null);// chuyển đổi một  đối tượng từ kiểu TSource sang kiểu TDestination
        IList<TDestination> Map<TDestination, TSource>(IList<TSource> sources, string? ignore = null);//chuyển đổi một danh sách các đối tượng từ kiểu TSource sang kiểu TDestination
        TDestination Map<TDestination>(object soure, string? ignore = null);//chuyển đổi một đối tượng từ kiểu nguồn không xác định (dưới dạng object) sang kiểu đích TDestination
        IList<TDestination> Map<TDestination>(IList<object> sources, string? ignore = null);//chuyển đổi một danh sách các đối tượng từ kiểu object sang kiểu TDestination 

    }
}