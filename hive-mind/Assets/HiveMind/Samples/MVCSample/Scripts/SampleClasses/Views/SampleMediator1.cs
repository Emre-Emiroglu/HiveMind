using HiveMind.Core.MVC.Attributes;
using HiveMind.Core.MVC.Views;
using UnityEngine;

namespace HiveMind.Samples.MVCSample.SampleClasses.Views
{
    [Mediator(key: "MVCSample")]
    public class SampleMediator1 : Mediator<SampleView1>
    {
        #region Constructor
        public SampleMediator1(SampleView1 view) : base(view) => Debug.Log("Sample Mediator 1 constructed!");
        #endregion

        #region PostConstruct
        public override void PostConstruct() => Debug.Log("Sample Mediator 1 post constructed!");
        #endregion
    }
}
