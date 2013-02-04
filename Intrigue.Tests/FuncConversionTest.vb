
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class FuncConversionTest

    <TestMethod()> Public Sub Function_symbol_to_string()

        Assert.AreEqual(
            """a""",
            Interpreter.DoEval("(symbol->string 'a)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_to_string()

        Assert.AreEqual(
            """13""",
            Interpreter.DoEval("(->string 13)").ToString)
        Assert.AreEqual(
            """a""",
            Interpreter.DoEval("(->string 'a)").ToString)
    End Sub

    <TestMethod()> Public Sub Function_integer_and_float()

        Assert.AreEqual(
            "1",
            Interpreter.DoEval("integer 1.2").ToString)
        Assert.AreEqual(
            "1",
            Interpreter.DoEval("int 1.2").ToString)

        Assert.AreEqual(
            "1.0",
            Interpreter.DoEval("float 1").ToString)

        Dim ex As Exception = Nothing
        Try
            Interpreter.DoEval("float ""nada""")
        Catch ex
        End Try
        Assert.AreEqual("cannot apply 'float' on ""nada""", ex.Message)
    End Sub
End Class
