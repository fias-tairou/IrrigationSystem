# Irrigation System

Het Irrigation System is een slimme irrigatiecontroller die op basis van regenval en bodemvochtigheid bepaalt of irrigatie moet starten of stoppen. Het systeem gebruikt sensorgegevens en een weer-API om water te besparen en planten gezond te houden. Bij herhaalde fouten schakelt het systeem over naar een veilige modus om overbewatering te voorkomen.

## Inleiding

Dit document beschrijft de stappen die zijn gevolgd bij het ontwikkelen van het `Irrigation System` project. De workflow volgt het Test Driven Development (TDD) proces en is geoptimaliseerd om op gestructureerde wijze een robuust systeem te bouwen.

## Workflow: Opzetten van de structuur

### 1. Ontwerp en identificatie van componenten

Eerst hebben we het ontwerp opgesteld en de benodigde componenten geïdentificeerd. Het project is verdeeld in logische modules om de verantwoordelijkheid van elke klasse te scheiden:

- **Interfaces**: Contracten voor interactie met externe of interne systemen.
- **Core**: Beslissingsmodule waarin de logica zit.
- **Services**: Concrete implementaties voor sensoren en controllers.
- **Models**: Simpele objecten voor gegevensuitwisseling (bijv. `WeatherData`).

#### **Klassediagram (PlantUML)**

We hebben de relaties tussen de verschillende onderdelen geïllustreerd met een klassendiagram:Je kan ze in de solution dit werd gedaan via de ZOMBIES testing principe onderaan de tabel

| **Letter** | **Scenario**                                                 | **Verwachte Actie**                      |
| ---------- | ------------------------------------------------------------ | ---------------------------------------- |
| **Z**      | Geen regenval, bodemvocht boven drempel.                     | Doe niets.                               |
| **Z**      | Geen regenval, bodemvocht exact op drempel.                  | Doe niets.                               |
| **O**      | Regenval precies op de drempel.                              | Stop irrigatie.                          |
| **O**      | Bodemvocht onder de drempel.                                 | Start irrigatie.                         |
| **M**      | Regenval boven de drempel.                                   | Stop irrigatie.                          |
| **M**      | Regenval en bodemvocht beide op drempel.                     | Stop irrigatie.                          |
| **B**      | Regenval exact op de drempel.                                | Stop irrigatie.                          |
| **B**      | Bodemvocht exact op de drempel.                              | Doe niets.                               |
| **I**      | Controleer of sensoren worden aangeroepen.                   | Sensor methoden worden aangeroepen.      |
| **I**      | Controleer of irrigatiecontroller correct wordt aangeroepen. | Start of stop wordt correct aangeroepen. |
| **E**      | API faalt meerdere keren.                                    | Activeer Safe Mode.                      |
| **S**      | API herstelt na falen.                                       | Verlaat Safe Mode en hervat.             |
