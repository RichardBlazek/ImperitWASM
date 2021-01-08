import sqlite3, json

class Connector:
	def __init__(self, path, log):
		self.con=sqlite3.connect(path)
		self.cur=self.con.cursor()
	def run(self, sql, *args):
		return self.cur.execute(sql, tuple(args))
	def commit(self):
		self.con.commit()
	def close(self):
		self.con.close()


db=Connector('imperit.db', './import.log')

cmd = input()
while cmd:
	print('\n'.join(', '.join(repr(it) for it in x) for x in db.run(cmd)))
	db.commit()
	cmd = input()
	
db.close()
