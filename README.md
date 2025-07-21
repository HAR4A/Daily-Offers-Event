# Daily-Offers-Event

Overview

This project implements a dynamic in-game offer system for Unity-based applications. It provides time-limited special offers (e.g., rewarded ads or IAP bundles), persistent state across sessions, and dynamic configuration via Firebase Remote Config. Offers and related UI assets are loaded on-demand using Addressables, ensuring a modular, scalable, and maintainable codebase.

Tech Stack

Engine & Language: Unity (IL2CPP/Mono), C#

Dependency Injection: Zenject

Async Programming: UniTask (Cysharp.Threading.Tasks)

Reactive Extensions: UniRx

Asset Management: Unity Addressables

Remote Configuration: Firebase Remote Config

Persistence & Serialization: JSON.NET (Newtonsoft.Json) + FileStorage

Advertisement & Monetization: Unity Ads (Rewarded), Unity IAP (In-App Purchases)

External SDKs: Firebase (Remote Config, Analytics)

Architecture
