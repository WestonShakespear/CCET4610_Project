# CCET4610_Project

## Introduction

The main inspiration for this idea is to have the ability to manage the parts and sub-assemblies that are used in projects. I want to build a fully standalone application for Windows that will communicate with Solidworks.

This client program will allow for new projects to be created and old projects to be downloaded and/or managed. The main feature will be the versioning for components and the ability to re-use already designed or acquired components in new projects without messy file paths or organization.

For both company and personal data and file management having centralized and networked file servers is paramount for file security. One of the main features of this project will be a way to centralize all of my cad parts and files, while I will mainly focus on Solidworks integration.

## Problem Statement

When creating an assembly for a new project I often find myself navigating folders and looking for parts I’ve created in the past. It’s difficult to find or remember which part is the most current since I will update the details or configurations for a part when using it in a new project.

This is a problem I have been wanting to solve before I even started attending SUU. Coming from a programming background I am used to revisioning and collaboration tools like git and GitHub. While these solve the problem for programmers perfectly, these tools don’t fit the cad and design environment as well.

## Detailed Proposed Solution

I have found a couple different online resources for developing stand-alone applications for Solidworks. There are a set of .chm files in the Solidworks api directory, these feature lots of different code examples for direct access to the api. The main focus of this project will be to explore the possibilities and use of the SolidWorks api. By trying to solve the file management problem I can learn more about interfacing with various other design softwares.

The basic structure of the programs that will be created are as follows. The server-side application will be written in Python and will heavily rely on the Flask framework to create a basic RESTful api for the desktop-based application for communication. The server will store relevant project and file data in a sql database. Listed below are the basic features of the server-side program.

The .NET application will communicate with Solidworks via the COM api, this will allow for automatic file opening, and revisioning based on parts being saved. It will also have a graphical interface for file and project management. There should be some kind of interface for creating new projects and a way to add tags for the categorization of projects.

A successful project will look like an application that can be used for the central management of resources, this should permit uploading and downloading parts and assemblies as well as version control for these uploads

## Summary

The main inspiration for this idea is to have the ability to manage the parts and sub-assemblies that are used in projects. I want to build a fully standalone application for Windows that will communicate with Solidworks.  

For both company and personal data and file management having centralized and networked file servers is paramount for file security. One of the main features of this project will be a way to centralize all of my cad parts and files, while I will mainly focus on Solidworks integration.



# Using the library

## SW_Instance


## SW_DocMgr




# Using the application
