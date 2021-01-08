import sqlite3, json

with open('./Settings.json') as f:
	settings=json.loads(f.read())

con=sqlite3.connect('imperit.db')
cur=con.cursor()

tables={x[0] for x in cur.execute('''
SELECT name FROM sqlite_master
WHERE type IN ('table','view')
AND name NOT LIKE 'sqlite_%'
AND NOT name = '__EFMigrationsHistory'
ORDER BY 1;''')}

for table in tables:
	print(table + '\n' + '\n'.join(repr(x) for x in cur.execute('PRAGMA table_info(' + table + ');')))

print(settings)
input()