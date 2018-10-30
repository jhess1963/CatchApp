using Abp.Web.Mvc.Views;

namespace CatchApp.Web.Views
{
    public abstract class CatchAppWebViewPageBase : CatchAppWebViewPageBase<dynamic>
    {

    }

    public abstract class CatchAppWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected CatchAppWebViewPageBase()
        {
            LocalizationSourceName = CatchAppConsts.LocalizationSourceName;
        }
    }
}