# Hello-Game
Learning project for scripting

Cilj Igre: Uničiti vse sovražnikove (rdeče) enote in zgradbe.

#• Zgradbe: 
  ##◦ House: Vsaka hiša poveča največje število dovoljenih enot za 5, kot tudi proizvaja čez čas (eno enoto imenovano
    “vilen” na 10 sekund) nove enote, katere proizvaja vse dokler jih lahko, oz. Je dosežen population limit.
  ##◦ Barracks: V tej zgradbi se osnovne enote natrenirajo za vojskovanje, ter dobijo meč in ščit. Proizvaja enoto 
    imenovana “fajter”, ki je osnovna enota za vojskovanje. Vanjo se lahko pošlje katerokoli enoto.
  
#• Enote: 
  ##◦ Vilen: Gradbitelji, nabiralci.. naokoli hodijo z vilami v rokah, katere lahko tudi kolikor toliko uspešno 
    uporabljajo. V igri se nanje gleda kot na surovino, saj jih je potrebno poslati v barake, kjer se natrenirajo za 
    pravo vojskovanje. 
  ##◦ Fajter: Prvovrstni vojaki, njihovo življenje je posvečeno vojskovanju. Njihov namen je razbijati nasprotnikove 
    zgradbe in ubijati njihove enote.

#• Gameplay: 
  ##◦ Enote se izbira z potegom miške čez ekran. S tem se ustvari selection box in enote znotraj njega bodo izbrane. 
  ##◦ Če   ni nobena enota izbrana, se na spodnjem panelu v zgornje levem kotu izpiše niz “hello”, če pa je ena ali več
    se izpiše ime prve enote. 
  ##◦ Nove zgradbe lahko zgradimo tako da kliknemo na željeni gumb, nato še enkrat prtisnemo na
    teren in nova hiša/barake že stojijo. ◦ Izbranim enotam se poda ukaz tako, da z desnim klikom na miško kliknemo na
    željeno lokacijo. 
#• V igro je tudi importan A* Pathfinding Project, s pomočjo katerega sem lažje implementeral premike enot:
  http://arongranberg.com/astar/docs/index.php 
