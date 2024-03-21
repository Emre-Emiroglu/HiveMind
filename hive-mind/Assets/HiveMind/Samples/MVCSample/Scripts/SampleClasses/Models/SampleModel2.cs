using HiveMind.MVC.Attributes;
using HiveMind.MVC.Models;
using UnityEngine;

namespace HiveMind.MVCSample.SampleClasses.Models
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
