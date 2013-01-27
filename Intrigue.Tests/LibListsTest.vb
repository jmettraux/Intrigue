
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class LibListsTest

    <TestMethod()> Public Sub LibFunc_map()

        Assert.AreEqual(
            "(14 15 16)",
            Interpreter.DoEval("map (lambda (x) (+ x 10)) '(4 5 6)").ToString)
    End Sub

    <TestMethod()> Public Sub LibFunc_map_with_funcname()

        Assert.AreEqual(
            "(22 24 26)",
            Intrigue.Interpreter.DoEval(
                Util.NewString(
                    <![CDATA[
                        define (plus10 x) (+ x 10)
                        map plus10 '(12 14 16)
                    ]]>)).ToString)
    End Sub
End Class
