
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FuncConversionTest

    <TestMethod()> Public Sub Primitive_symbol_to_string()

        Assert.AreEqual(
            """a""",
            Interpreter.DoEval("(symbol->string 'a)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_to_string()

        Assert.AreEqual(
            """13""",
            Interpreter.DoEval("(->string 13)").ToString)
        Assert.AreEqual(
            """a""",
            Interpreter.DoEval("(->string 'a)").ToString)
    End Sub

    <TestMethod()> Public Sub Primitive_inexact()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("inexact->exact 1.2").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("inexact->exact 1.2").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("->exact 1.2").ToString)

        Assert.AreEqual(
            "1.0",
            Interpreter.DoEval("exact->inexact 1").ToString)
        Assert.AreEqual(
            "1.0",
            Interpreter.DoEval("->inexact 1").ToString)

        Dim ex As Exception = Nothing
        Try
            Interpreter.DoEval("exact->inexact ""nada""")
        Catch ex
        End Try
        Assert.AreEqual("cannot apply 'exact->inexact' on ""nada""", ex.Message)
    End Sub
End Class
