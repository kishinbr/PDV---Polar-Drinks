# Polar Drinks
<div align="center">
👨‍💻 Desenvolvido por

**[Kishin](https://github.com/kishinbr)**

</div>

---

<div align="center">

<img src="PolarDrinks/wwwroot/img/TELA-LOGIN.png" alt="Polar Drinks Logo" width="100%"/>

# 🥤 Polar Drinks — Sistema de PDV (Ponto de Venda)

**Sistema de PDV completo para gerenciar vendas de bebidas de forma rápida e prática.**
Registre vendas, controle produtos e estoque, acompanhe clientes e gere relatórios — tudo em um só lugar.

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?style=for-the-badge&logo=bootstrap)

</div>

---

## ✨ Funcionalidades

| Módulo | Descrição |
|--------|-----------|
| 🔐 **Login** | Autenticação segura de acesso ao sistema |
| 📊 **Dashboard** | Visão geral de vendas, faturamento e movimentação |
| 🛒 **Vendas (PDV)** | Registro rápido de vendas no ponto de venda |
| 🥤 **Produtos** | Catálogo de bebidas com código, preço e estoque |
| 📦 **Estoque** | Controle de entrada e saída de produtos |
| 👥 **Usuários** | Cadastro e edição de usuários |
| 📄 **Movimentações** | Geração de relatórios de vendas e movimentações |


---

## 🖥️ Telas do Sistema


### 🔑 Login
<img src="PolarDrinks/wwwroot/img/TELA-LOGIN.png" alt="Tela de Login" width="100%"/>

---

### 📊 Dashboard
<img src="PolarDrinks/wwwroot/img/TELA-DASHBOARD.png" alt="Dashboard" width="100%"/>

---

### 🛒 Ponto de Venda (PDV)
<img src="PolarDrinks/wwwroot/img/TELA-PDV.png" alt="Tela de PDV" width="100%"/>

---

### 🥤 Produtos
<img src="PolarDrinks/wwwroot/img/TELA-PRODUTOS.png" alt="Produtos" width="100%"/>

---

### 👥 Usuários
<img src="PolarDrinks/wwwroot/img/TELA-USUARIOS.png" alt="Clientes" width="100%"/>

---

### 📄 Movimentações
<img src="PolarDrinks/wwwroot/img/TELA-MOVIMENTACOES.png" alt="Relatórios" width="100%"/>

---

## 🏗️ Arquitetura do Projeto

```
PolarDrinks/
├── Controllers/          # Controllers (Vendas, Produtos, Clientes, Estoque, Login)
├── Data/
│   ├── ApplicationDbContext.cs   # Contexto do Entity Framework
│   └── DbInitializer.cs          # Seed automático de dados iniciais
├── Migrations/           # Migrations do Entity Framework Core
├── Models/                # Entidades do banco de dados
├── Repositories/          # Padrão Repository (acesso ao banco)
├── Services/              # Lógica de negócio
├── Views/                 # Views (frontend)
└── wwwroot/                # Arquivos estáticos (CSS, JS, imagens)
```


---

## 🚀 Como Rodar Localmente

### Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (ou SQL Server Express)
- [Git](https://git-scm.com/)

---

### 1. Clonar o Repositório

```bash
git clone https://github.com/kishinbr/PDV---Polar-Drinks.git
cd PDV---Polar-Drinks
```

---

### 2. Configurar a Connection String

Abra o arquivo `appsettings.json` e altere a connection string para o seu banco de dados local:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR; Database=PolarDrinksDB; Trusted_Connection=True; TrustServerCertificate=True;"
}
```

> 💡 Se estiver usando SQL Server local com Windows Authentication, basta substituir `SEU_SERVIDOR` pelo nome da sua instância (ex: `localhost` ou `DESKTOP-XXXX`).

---

### 3. Restaurar Dependências

```bash
dotnet restore
```

---

### 4. Rodar o Projeto

```bash
dotnet run
```

Acesse: **http://localhost:5000**

> ⚠️ Confirme a porta correta que aparece no terminal ao rodar o projeto.

---

### 5. Login Padrão

```
Usuário: admin
Senha:   admin123
```


---

## 📦 Dependências Principais

| Pacote | Versão | Uso |
|--------|--------|-----|
| `Microsoft.EntityFrameworkCore.SqlServer` | - | ORM para SQL Server |
| `Microsoft.EntityFrameworkCore.Tools` | - | Migrations e CLI |
| `Bootstrap` | 5 | Interface responsiva |



---

## ☁️ Deploy (Hospedagem)

Para hospedar em produção:

```bash
dotnet publish -c Release -o ./publish
```

Compacte a pasta `publish/` e faça o upload no seu servidor (ex: Somee.com, Azure App Service, etc.).

---

## 👨‍💻 Desenvolvido por

<div align="center">

**[Kishin](https://github.com/kishinbr)**

</div>

---


