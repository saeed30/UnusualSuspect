using System.Linq;

namespace UnusualSuspect.ApiViewModels.InputParameters
{
	public class PagingFilterParamters
	{
		public int? Take { get; set; }
		public int? Page { get; set; }
		public int? OrderType { get; set; }

		public void SetDefault()
		{
			Page = Page ?? 1;
			Take = Take ?? 50;
			OrderType = OrderType ?? 1;
		}
		public IQueryable<TSource> Paging<TSource>(IQueryable<TSource> source, PagingFilterParamters paging)
		{
			paging.SetDefault();
			return source.Skip((paging.Page.Value - 1) * paging.Take.Value).Take(paging.Take.Value);
		}

	}
}