
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class LibMathTest

    <TestMethod()> Public Sub LibFunc_gcd()

        Assert.AreEqual(
            "4",
            Interpreter.DoEval("gcd 8 4").ToString)
        Assert.AreEqual(
            "13828",
            Interpreter.DoEval("gcd (* 3457 12) (* 3457 8)").ToString)

        Assert.AreEqual(
            "4",
            Interpreter.DoEval("gcd 32 -36").ToString)
        Assert.AreEqual(
            "4.0",
            Interpreter.DoEval("gcd 32.0 -36").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_gcd_zero()

        Assert.AreEqual(
            "0",
            Interpreter.DoEval("(gcd)").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_gcd_multiple()

        Assert.AreEqual(
            "5",
            Interpreter.DoEval("gcd 30 40 35").ToString)
    End Sub
End Class
