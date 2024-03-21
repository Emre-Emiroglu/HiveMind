using HiveMind.MVC.Attributes;
using HiveMind.MVC.Datas;

namespace HiveMind.MVC.Binders
{
    public sealed class MediationBinder : Binder<MediatorAttribute>
    {
        #region Constructor
        public MediationBinder(BinderData binderData) : base(binderData) { }
        #endregion

        #region Bindings
        public override void Bind() => base.Bind();
        #endregion
    }
}
