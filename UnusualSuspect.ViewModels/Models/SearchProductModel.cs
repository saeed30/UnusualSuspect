using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UnusualSuspect.ViewModels.Models;

    public class SearchProductModel : BaseViewModel
    {
        public SearchProductModel()
        {

            SortTypeList = new List<SelectListItem>();
            SortTypeList.Add(new SelectListItem() { Value = "1", Text = "مرتبط ترین" });
            SortTypeList.Add(new SelectListItem() { Value = "2", Text = "گرانترین" });
            SortTypeList.Add(new SelectListItem() { Value = "3", Text = "ارزانترین" });
            SortTypeList.Add(new SelectListItem() { Value = "4", Text = "قدیمی ترین" });
            SortTypeList.Add(new SelectListItem() { Value = "5", Text = "جدیدترین" });
            Fillters = new List<Searchfillter>();
            PageSize = 18;
            Step = 1;
        }
        public List<Searchfillter> Fillters { set; get; }

        [Display(Name = "کمترین قیمت ")]
        public long? MinPriceTwo { set; get; }

        [Display(Name = "بیشترین قیمت ")]
        public long? MaxPriceTwo { set; get; }

        /// <summary>
        /// موجود / ناموجود
        /// </summary>
        public bool? Existent { set; get; }

        public int? CategoryId { set; get; }

        public string FillterListJson { set; get; }

        public string Fillter { set; get; }
        public int? ListOrTable { get; set; }
        public string RangeResult { set; get; }
        public string searchTitle { set; get; }
    }
    public class Searchfillter
    {
        /// <summary>
        /// نوع آیتم مورد جستجو را مشخص میکند
        /// </summary>
        public string ItempType { set; get; }
        public int ItemId { set; get; }
        public int? FeildId { set; get; }
        public string Value { set; get; }
    }
    public class TotalProductFillter
    {
        /// <summary>
        /// نوع فیلد
        /// </summary>
        public string ItemType { get; set; }
        /// <summary>
        /// عنوان فیلد
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// مکان قرارگیری
        /// </summary>
        public int LocationInList { set; get; }
        public int ItemId { set; get; }
        public List<TEFillterItem> TEFillterItems { set; get; }
    }
    public class TEFillterItem
    {
        public bool selected { set; get; }

        public string Value { set; get; }
        public int ValueId { set; get; }
        public bool Selected { set; get; }

        public string Text { set; get; }
        public int? ItemId { get; set; }
        public string Unit { get; set; }
    }
