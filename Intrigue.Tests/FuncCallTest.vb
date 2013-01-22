
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FuncCallTest

    <TestMethod()> Public Sub Function_call()

        Assert.AreEqual(
            """fox""",
            Intrigue.Interpreter.DoEval(
                <![CDATA[
                    define a " fox "
                    call a "Trim"
                ]]>.ToString.Trim
            ).ToString)
    End Sub
End Class
