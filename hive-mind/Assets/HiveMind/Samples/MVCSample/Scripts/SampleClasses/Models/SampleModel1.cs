using HiveMind.Core.MVC.Attributes;
using HiveMind.Core.MVC.Models;
using UnityEngine;

namespace HiveMind.Samples.MVCSample.SampleClasses.Models
{
    [Model(key: "MVCSample")]
    public class SampleModel1 : Model<SampleModelSettings1>
    {
        #region Constructor
        public SampleModel1() : base(string.Empty) => Debug.Log("Sample Model 1 constructed!");
        #endregion

        #region PostConstruct
        public override void PostConstruct() => Debug.Log("Sample Model 1 post constructed!");
        #endregion
    }
}
