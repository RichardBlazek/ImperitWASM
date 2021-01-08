import sqlite3

con=sqlite3.connect('imperit.db')
cur=con.cursor()

tables={x[0] for x in cur.execute('''
SELECT name FROM sqlite_master 
WHERE type IN ('table','view') 
AND name NOT LIKE 'sqlite_%'
AND NOT name = '__EFMigrationsHistory'
ORDER BY 1;''')}

print(tables)
input()