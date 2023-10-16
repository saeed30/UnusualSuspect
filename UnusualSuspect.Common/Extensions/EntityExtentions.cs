//using UnusualSuspect.ViewModels.Api.InputParameters;
//using System.Linq;

//namespace UnusualSuspect.Common.Extensions;
//    public static class EntityExtentions
//    {
//        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, PagingFilterParamters paging)
//        {
//            paging.SetDefault();
//            return source.Skip((paging.Page.Value - 1) * paging.Take.Value).Take(paging.Take.Value);
//        }
//    }
