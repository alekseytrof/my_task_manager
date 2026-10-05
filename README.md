# MyTaskManager

A full-featured task management system built with **C# / WPF**, consisting of a **server** (REST API) and a **desktop client** — both included in a single Visual Studio solution.

MyTaskManager helps you organize your work using **projects**, **boards**, **columns**, and **tasks**, with role-based access for regular users and administrators.

<!--
  PHOTO HERE — Main hero image / logo / banner
  Description: A wide banner or logo of the project to give a first impression.
  Design rating: ★★★★★ — a clean, branded hero image significantly improves
  the perceived quality of the README and encourages users to keep reading.
-->

---

## 📋 Table of Contents

- [Features](#-features)
- [Screenshots](#-screenshots)
- [Architecture](#-architecture)
- [Requirements](#-requirements)
- [Technologies](#-technologies)
- [Author](#-author)

---

## ✨ Features

- 🔐 **Authentication** — secure login with username and password
- ⚡ **Quick login** — one-click sign-in for the last used account
- 📁 **Projects** — group your work into projects
- 🗂 **Boards** — organize projects into multiple boards
- 📌 **Columns & Tasks** — drag and drop tasks between columns
- 👥 **User Management** — administrators can view and manage all users
- 🖥 **Desktop client** — native Windows application built with WPF
- 🌐 **REST API server** — separate backend for data storage and communication

---

## 📸 Screenshots

### 1. Registration / Login Window

![Login window]([https://github.com/user/repo/assets/12345/abcd-1234](https://github.com/alekseytrof/my_task_manager/issues/1#issue-5708112518))
<img width="382" height="289" alt="Image" src="https://github.com/user-attachments/assets/36e23d56-f345-4e9c-81a3-d60a50bf65b7" />

Description: Displays the login form with fields for username and password,

plus a "Login as last user" quick-access button.

A clean and minimal login screen; consider

adding your logo at the top for extra brand recognition.

### 2. Main Window

<!--
  PHOTO HERE — Main window
  Description: The central hub of the application, with tabs for Projects,
  Boards, Tasks (and Users for administrators), plus an Exit button.
  Design rating: ★★★★★ — a well-structured layout with clear navigation;
  the tabbed interface makes switching between sections fast and intuitive.
-->

### 3. Projects Window

<!--
  PHOTO HERE — Projects view
  Description: Shows all available projects and allows creating, editing,
  and deleting them.
  Design rating: ★★★★☆ — functional and clear; adding project icons or
  color tags would improve visual differentiation.
-->

### 4. Boards Window

<!--
  PHOTO HERE — Boards view
  Description: Displays boards belonging to the selected project.
  Design rating: ★★★★☆ — consistent with the projects view;
  consider a card-based layout for better visual appeal.
-->

### 5. Columns & Tasks Window

<!--
  PHOTO HERE — Columns and tasks (Kanban-style) view
  Description: The core workspace — columns containing tasks that can be
  freely dragged and dropped between them.
  Design rating: ★★★★★ — the Kanban-style drag-and-drop interaction is the
  standout feature; smooth animations would make it even better.
-->

### 6. User Management Window (Admin only)

<!--
  PHOTO HERE — Users management view
  Description: Available only to administrators. Allows viewing, editing,
  and managing all registered users of the system.
  Design rating: ★★★★☆ — good separation of admin functionality;
  consider adding search and filtering for larger user lists.
-->

---

## 🏗 Architecture

The solution contains two main projects:

MyTaskManager.sln

│

├── MyTaskManager.Server → REST API backend (data storage, authentication)

└── MyTaskManager.Client → WPF desktop application (UI)

The **client** communicates with the **server** over HTTP using a REST API.

---

## ✅ Requirements

- **Windows 10 / 11**
- **.NET SDK** (version 8.0 or later)
- **Visual Studio 2022** (or any IDE that supports .NET and WPF)
- **PostgreSQL**

---

## 🛠 Technologies

- C#

- .NET (WPF for the client)

- ASP.NET Core Web API (server)

- Entity Framework Core (data access — adjust if different)

- XAML (UI markup)

- REST API (client–server communication)
  
---

## 👤 Author

Aleksey Trofimov

GitHub: @alekseytrof

Email: alekcey.trofimov.99@mail.ru
