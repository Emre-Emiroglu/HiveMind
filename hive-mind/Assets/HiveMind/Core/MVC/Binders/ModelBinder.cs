using HiveMind.MVC.Attributes;
using HiveMind.MVC.Datas;

namespace HiveMind.MVC.Binders
{
    public sealed class ModelBinder : Binder<ModelAttribute>
    {
        #region Constructor
        public ModelBinder(BinderData binderData) : base(binderData) { }
        #endregion

        #region Bindings
        public override void Bind() => base.Bind();
        #endregion
    }
}
