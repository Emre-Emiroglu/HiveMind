using System;

namespace HiveMind.MVC.Controller
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CommandInjectAttribute : Attribute
    {
    }
}