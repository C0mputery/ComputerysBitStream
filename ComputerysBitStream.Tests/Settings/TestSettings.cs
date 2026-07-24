using ComputerysBitStream.Attributes;
using ComputerysBitStream.Extras.Settings;

[assembly: DefaultBitStreamSettings(typeof(ComputerysBitStream.Tests.Settings.ITestSettings))]

namespace ComputerysBitStream.Tests.Settings;

[BitStreamSettings]
public interface ITestSettings : IGameExtrasSettings { }
