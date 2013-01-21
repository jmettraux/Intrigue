
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class NumericFunctionsTest

    <TestMethod()> Public Sub Function_plus()

        Assert.AreEqual(
            "5",
            Interpreter.DoEval("(+ 2 3)").ToString)
        Assert.AreEqual(
            "5",
            Interpreter.DoEval("+ 2 3").ToString)
    End Sub
End Class
