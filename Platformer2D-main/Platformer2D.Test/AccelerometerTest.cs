using Microsoft.Xna.Framework;
namespace Platformer2D.Test;

public class AccelerometerTest
{
    /**
    * Accelerometer reset to default state after every test
    */
    [TearDown]
    public void Cleanup()
    {
        Accelerometer.Reset();
    }
    /**
    * Accelerometer thrown exception when use without initialize
    */
    [Test]
    public void Accelerometer_GetStateExpectException_WhenUseWithoutInitialize()
    {
        var thrown = Assert.Throws<InvalidOperationException>(() => Accelerometer.GetState());
        Assert.That(thrown?.Message, Is.EqualTo("You must Initialize before you can call GetState"));
    }

    /**
    * Accelerometer thrown exception when initialize twice
    */
    [Test]
    public void Accelerometer_ExpectObject_ToBeInitializeOnlyOnce()
    {
        Accelerometer.Initialize();
        var thrown = Assert.Throws<InvalidOperationException>(() => Accelerometer.Initialize());
        Assert.That(thrown?.Message, Is.EqualTo("Initialize can only be called once"));
    }

    /**
    * Accelerometer initialize success
    */
    [Test]
    public void Accelerometer_GetStateExpect_ReturnNewAccelerometerState()
    {
        Accelerometer.Initialize();
        var state1 = Accelerometer.GetState();
        var state2 = Accelerometer.GetState();
        Assert.That(state1, Is.EqualTo(state2));
        Console.WriteLine(state1.Equals(state2));
        Assert.That(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(state1),
            Is.Not.EqualTo(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(state2)));
    }
    
    /**
    * Accelerometer initialize success with different acceleration and active status
    */
    [Test]
    public void AccelerometerState_ExpectInitializeWithDifferentAccelerationAndActiveStatus_ToBeSuccess()
    {
        var acceleration = new Vector3(new Vector2(), 10);
        var accelerometerState = new AccelerometerState(acceleration, true);
        Assert.That(accelerometerState.IsActive, Is.True);
        Assert.That(accelerometerState.Acceleration, Is.EqualTo(acceleration));

        accelerometerState = new AccelerometerState(new Vector3(), false);
        Assert.That(accelerometerState.IsActive, Is.False);
        Assert.That(accelerometerState.Acceleration, Is.Not.EqualTo(acceleration));
    }

    /**
    * Accelerometer toString match predefine format
    */
    [Test]
    public void AccelerometerState_ExpectToString_ToMatchPreDefineFormat()
    {
        var accelerometerState = new AccelerometerState(new Vector3(new Vector2(), 10), false);
        var actual = accelerometerState.ToString();
        Assert.That(actual, Is.EqualTo("Acceleration: {X:0 Y:0 Z:10}, IsActive: False"));
    }
}