# Daily Offers Event

A time-limited in-game offer system for Unity applications.Includes dynamic offers, persistent state, remote configuration, and on-demand UI loading.

**Unity version used:** Unity 2022.3.59f1

## Features

- Time-limited special offers via rewarded ads or in-app purchase bundles

- Persistent event state across game sessions

- Dynamic configuration through Firebase Remote Config

- On-demand UI and asset loading using Addressables

- Assembly Definitions: code organized into asmdef assemblies (Config, Services, Events, UI, Core) for reusable modules and faster incremental builds.

## Architecture Overview

- EntryPoint (Bootstrapper): initializes all IInitializeService implementations in startup sequence.

- Config Loading: JsonConfigProvider aggregates FirebaseConfigRepository (Remote Config) and LocalConfigRepository (Offers.json) into a unified OfferConfig list.

- Purchase Flow: PurchaseService abstracts Rewarded Ad and IAP logic, exposing a single OnPurchase(offerId) event.

- Event Management: OfferEvent controls the offer lifecycle, persisting EventState via FileStorage and scheduling expiration with TimerService.

- UI Layer: AddressablesOfferUIViewFactory dynamically loads UI prefabs from Addressables; UI components use UniRx for reactive interactions.

- Dependency Injection: Zenject separates configuration, services, and UI for modularity and testability.


## Technology Stack

- Zenject – Dependency Injection framework

- UniTask – Async/await support in Unity

- UniRx – Reactive Extensions for user interface

- Addressables – Asset bundle management

- Firebase Remote Config – Dynamic remote settings

- Unity Ads (Rewarded) and Unity IAP – Monetization services

- Newtonsoft.Json (Json.NET) – JSON serialization and deserialization
