
# iBay

## Description

iBay est une plateforme de commerce électronique moderne conçue pour faciliter le commerce en ligne à travers une API RESTful. Elle permet la gestion des comptes utilisateurs, des produits, des paniers d'achat, des commandes, et des contacts de manière efficace et sécurisée.

## Installation

Clonez le dépôt :

```bash
git clone https://github.com/Younes-baddou/Ibay.git
```

Accédez au répertoire du projet et installez les dépendances nécessaires :

```bash
cd Ibay
dotnet restore
```

Appliquez les migrations et créez la base de données :

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Lancez l'application :

```bash
dotnet run
```

## Configuration

Configurez les paramètres nécessaires dans `appsettings.json`, y compris les chaînes de connexion à la base de données et les clés secrètes pour l'authentification JWT.

## Fonctionnalités & Endpoints

- Gestion des utilisateurs : Inscription, authentification, et gestion des profils utilisateurs.
Inscription : `POST /api/account/register`
    ```json
    {
      "firstName": "string",
      "lastName": "string",
      "email": "user@example.com",
      "phone": "string",
      "address": "string",
      "password": "stringst"
    }

- Gestion des produits : Ajout, mise à jour, et suppression de produits.
 - Ajouter un produit : `POST /api/Products`
    ```json
    {
      "Name": "string",
      "Brand": "string",
      "Category": "string",
      "Price": 0,
      "Description": "string",
      "ImageFile": "string"
    }

- Gestion du panier d'achat : Ajout et suppression d'articles dans le panier, visualisation du panier.

- Gestion des commandes : Passation et suivi des commandes.
- Créer une commande : `POST /api/orders`
    ```json
    {
      "productIdentifiers": "string",
      "deliveryAddress": "stringstringstringstringstring",
      "paymentMethod": "string"
    }

- Gestion des contacts : Envoi de messages ou demandes d'informations via un formulaire de contact.
 Envoyer un message : `POST /api/contacts`
    ```json
    {
      "firstName": "string",
      "lastName": "string",
      "email": "user@example.com",
      "phone": "string",
      "subjectId": 0,
      "message": "stringstringstringst"
    }

- Authentification
L'API utilise JSON Web Tokens (JWT) pour l'authentification. Assurez-vous d'inclure le jeton dans l'en-tête de vos requêtes sécurisées.


## Documentation de l'API

Pour une liste complète des endpoints et leur documentation, veuillez consulter [Swagger UI](https://localhost:4000/swagger/index.html) après avoir lancé l'application.

