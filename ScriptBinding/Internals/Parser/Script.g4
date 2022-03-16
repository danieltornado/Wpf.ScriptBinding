grammar Script;

/****************/
/* Parser Rules */
/****************/

root
	: statement EOF
;

statement
	: additivePositive
	| conditional
	| notCondition
	| memberAccess
	| string
	| number
;

additivePositive
	: subtracting ('+' subtracting)*
;

subtracting
	: multiplicative ('-' multiplicative)*
;

multiplicative
	: dividing ('*' dividing)*
;

dividing
	: mod ('/' mod)*
;

mod
	: operandOr ('%' operandOr)*
;

operandOr
	: operandAnd ('or' operandAnd)*
;

operandAnd
	: condition ('and' condition)*
;

condition
	: arithmeticArgument ((GREATER|GREATER_EQUALS|LESS|LESS_EQUALS|EQUALS|NOT_EQUALS) arithmeticArgument)?
;

arithmeticArgument
	: conditional
	| notCondition
	| memberAccess
	| string
	| number
;

conditional
	: IF OPEN_PARENS statement CLOSE_PARENS THEN OPEN_PARENS statement CLOSE_PARENS ELSE OPEN_PARENS statement CLOSE_PARENS
;

notCondition
	: NOT OPEN_PARENS statement CLOSE_PARENS
;

memberAccess
	: (member | parensStatement) (DOT member)*
;

member
	: invoke
	| identifier 
;

invoke
	: identifier OPEN_PARENS (invokeParameter (COMMA invokeParameter)*)? CLOSE_PARENS
;

invokeParameter
	: statement
;

parensStatement
	: OPEN_PARENS statement CLOSE_PARENS
;

identifier
	: IDENTIFIER
;

string
	: STRING_1
	| STRING_2
;

number
	: INTEGER '.' INTEGER ('d' | 'D' | 'f' | 'F' | 'm' | 'M')?
	| INTEGER ('d' | 'D' | 'f' | 'F' | 'm' | 'M' | 'l' | 'L')?
;

/***************/
/* Lexer Rules */
/***************/

IF				: 'if'
;
THEN			: 'then'
;
ELSE			: 'else'
;
NOT				: 'not'
;
STRING_1		: '"' (~('"'))* '"'
;
STRING_2		: '\'' (~('\''))* '\''
;
IDENTIFIER		: (('a'..'z') | ('A'..'Z') | '_')+ (('0'..'9') | ('a'..'z') | ('A'..'Z') | '_')*
;
INTEGER			: ('0'..'9')+
;
OPEN_PARENS		: '('
;
CLOSE_PARENS	: ')'
;
DOT				: '.'
;
COMMA			: ','
;
GREATER			: '>'
;
GREATER_EQUALS	: '>='
;
LESS			: '<'
;
LESS_EQUALS		: '<='
;
EQUALS			: '=='
;
NOT_EQUALS		: ('!='|'<>')
;
WHITESPACES		: ' '+ -> channel(HIDDEN)
;