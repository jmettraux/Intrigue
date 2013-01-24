
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
    (map f l)
    (if (empty? l)
      l
      (cons (f (car l)) (map f (cdr l)))))

  (map (lambda (x) (+ x 2)) '(10 20 30))

        ]]>)
).ToString

' yields "(12 22 32)"
```

Intrigue is indentation-sensitive, which lets one drop a few parentheses, so the above example can be rewritten as:

```vb
Imports Intrigue

Intrigue.Interpreter.DoEval(
    Intrigue.Util.NewString(
        <![CDATA[

  define
    map f l
    if (empty? l)
      l     ; then
      cons  ; else
        f (car l)
        map f (cdr l)

  map
    lambda (x) (+ x 2)
    '(10 20 30)

        ]]>)
).ToString

' yields "(12 22 32)"
```

### setting initial interpreter data

TODO


### binding one's own builtin functions

TODO


## builtin 'functions'

(more on the way)

### true
the equivalent of #t in a true Scheme

### false
same for #f

### quote
quotes a list ```'(1 2 3)``` is equivalent to ```(quote 1 2 3)```

### define
binds a variable in the current context, supports the traditional Scheme to bind a function

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

### + = < <= > >=
as expected

### call - calls a .NET method on the target atom:

for example:

```scheme
(define a " fox ")
(call a "Trim")

; yields "fox"
```


## license

MIT (see LICENSE.txt)
