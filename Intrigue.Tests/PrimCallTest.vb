
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FuncCallTest

    <TestMethod()> Public Sub Primitive_call()

        Assert.AreEqual(
            """fox""",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define a " fox "
                        call a "Trim"
                    ]]>)).ToString)
    End Sub
End Class
