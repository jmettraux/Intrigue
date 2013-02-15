
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class JsonTest

    <TestMethod()> Public Sub Json_array()

        Assert.AreEqual(
            "(1 2 3 4)",
            Interpreter.DoEval("[ 1, 2, 3, 4 ]").ToString)
    End Sub
End Class
