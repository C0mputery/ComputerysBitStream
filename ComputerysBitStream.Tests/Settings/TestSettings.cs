using ComputerysBitStream.Attributes;

[assembly: DefaultBitStreamSettings(typeof(ComputerysBitStream.Tests.Settings.ITestSettings))]

namespace ComputerysBitStream.Tests.Settings;

[BitStreamSettings]
public interface ITestSettings : IDefaultSettings { }
