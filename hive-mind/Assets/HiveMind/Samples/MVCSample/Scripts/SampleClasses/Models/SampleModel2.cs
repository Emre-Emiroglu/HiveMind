using HiveMind.Core.MVC.Attributes;
using HiveMind.Core.MVC.Models;
using UnityEngine;

namespace HiveMind.Samples.MVCSample.SampleClasses.Models
{
    [Model(key: "MVCSample")]
    public class SampleModel2 : Model<SampleModelSettings2>
    {
        #region Constructor
        public SampleModel2() : base(string.Empty) => Debug.Log("Sample Model 2 constructed!");
        #endregion

        #region PostConstruct
        public override void PostConstruct() => Debug.Log("Sample Model 2 post constructed!");
        #endregion
    }
}
