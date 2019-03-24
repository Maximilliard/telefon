package hu.telefon.model.service;

public class Ido {

	public int Ora;
	public int Perc;
	public int MP;
	
	public int getOra() {
		return Ora;
	}

	public void setOra(int ora) {
		Ora = ora;
	}

	public int getPerc() {
		return Perc;
	}

	public void setPerc(int perc) {
		Perc = perc;
	}

	public int getMP() {
		return MP;
	}

	public void setMP(int mP) {
		MP = mP;
	}

	public Ido(int ora, int perc, int mP) {
		Ora = ora;
		Perc = perc;
		MP = mP;
	}
	
	 public int mpTicks() {
         return 60 * 60 * Ora + 60 * Perc + MP;
     }
}
