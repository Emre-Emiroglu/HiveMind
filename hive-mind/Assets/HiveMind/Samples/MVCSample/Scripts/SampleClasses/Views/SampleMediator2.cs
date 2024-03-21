using HiveMind.MVC.Attributes;
using HiveMind.MVC.Views;
using UnityEngine;

namespace HiveMind.MVCSample.SampleClasses.Views
{
    [Mediator(key: "MVCSample")]
    public class SampleMediator2 : Mediator<SampleView2>
    {
        #region Constructor
        public SampleMediator2(SampleView2 view) : base(view) => Debug.Log("Sample Mediator 2 constructed!");
        #endregion

        #region PostConstruct
        public override void PostConstruct() => Debug.Log("Sample Mediator 2 post constructed!");
        #endregion
    }
}
