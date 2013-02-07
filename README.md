
# intrigue

A mini Scheme for the .NET platform.

Not intended to be complete, awesome or to conquer the world. Just a tool.


## installation

It's available via [NuGET](http://nuget.org/packages/Intrigue/).


## usage

Here is an example of the "map" method getting implemented and then applied on a ```'(10 20 30)``` list.

```vb
Imports Intrigue

Intrigue.Interpreter.DoEval(
    Intrigue.Util.NewString(
        <![CDATA[

  (define
    (mymap f l)
    (if (empty? l)
      l
      (cons (f (car l)) (mymap f (cdr l)))))

  (mymap (lambda (x) (+ x 2)) '(10 20 30))

        ]]>)
).ToString

' yields "(12 22 32)"
```

Intrigue is indentation-sensitive, which lets one drop a few parentheses, so the above example can be rewritten as:

```scheme
  define
    mymap f l
    if (empty? l)
      l     ; then
      cons  ; else
        f (car l)
        mymap f (cdr l)

  mymap
    lambda (x) (+ x 2)
    '(10 20 30)
```

### setting initial interpreter data

There are two shortcut methods available ```BindAtom``` and ```LookupAtom``` to wrap/fetch data in an Intrigue interpreter.

```vb
Dim i = New Intrigue.Interpreter

i.BindAtom("a", 3)

i.Lookup("a").ToString  ' yields "3"
i.LookupAtom("a")       ' yields 3
```

TODO: what about ListNodes?


### binding one's own builtin functions

One can implement a function in VB or C# (or whatever .NET supports) and bind it in the interpreter.

```vb
Imports Intrigue
Imports Intrigue.Nodes

Public Class UpcaseFunction
    Inherits Intrigue.Nodes.FunctionNode

    Public Overrides Function Apply(funcName As String, ByRef args As ListNode, ByRef env As Context) As Node

        ' will raise if there isn't 1 and only 1 argument
        CheckArgCount(funcName, args, 1)

        ' eval argument and then extract value
        Dim s0 = DirectCast(env.Eval(args.Car).ToAtomNode.Atom, String)

        ' call ToUpper on argument string
        Dim s1 = s0.ToUpper

		' wrap result in an AtomNode
        Return New AtomNode(s1)
    End Function
End Class

' and, when preparing the interpreter...

Dim i = New Intrigue.Interpreter
i.Bind("upcase", New UpcaseFunction)

i.Eval("(upcase ""London"")").ToString ' will yield '"LONDON"'
```

For more, look at the Func*.vb files to see how the builtin functions are implemented.

Note that the functions have to explicitely evaluate their arguments, via the env.


## builtin 'functions'

(more on the way)

### true
the equivalent of #t in a true Scheme

### false
same for #f

### quote
quotes a list ```'(1 2 3)``` is equivalent to ```(quote 1 2 3)```

### define
binds a variable in the current env, supports the traditional Scheme to bind a function

### lambda
packages an anonymous function

### if
control flow

### cond
control flow

### equal?
evaluates to true if both args have the same string representation

### atom?
returns true if the argument is not a list

### list?
returns true if the argument is a list

### number? boolean? string? exact? inexact?
as expected

### car
returns the head of a list

### cdr
returns a new list (all the elts of the arg list except the head)

### cons
```(cons 1 '(2 3))``` yields ```(1 2 3)```

### empty?
returns true if the arg is a list and is empty

### any?
returns true if the arg is a list and has at least one element

### and or not
combines boolean results, exits as soon as the final answer can be determined (potentially doesn't evaluate all the arguments)

### + - / * = < <= > >=
as expected

### float integer
convert to float and integer respectively;
will probably need to introduce "exact->inexact" and vice versa.

### symbol->string ->string
as expected

### exact->inexact inexact->exact ->inexact ->exact
as expected

### zero? positive? negative? odd? even?
as expected

### min max
given a list return the minimum number or the max one respectively. If a least one number is inexact, the result will be inexact (Double).

### call
calls a .NET method on the target atom:

for example:

```scheme
(define a " fox ")
(call a "Trim")

; yields "fox"
```

### map

```scheme
define (plus10 x) (+ x 10)
map plus10 '(12 14 16)
  ; yields (22 24 26)
```

### eval
the classical Scheme/Lisp eval

### the-environment
returns the current environment


## issues, support and discussions

https://github.com/jmettraux/Intrigue/issues


## license

MIT (see LICENSE.txt)
