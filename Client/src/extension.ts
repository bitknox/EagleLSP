/* --------------------------------------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 * ------------------------------------------------------------------------------------------ */
// tslint:disable
"use strict";

import { workspace, ExtensionContext } from "vscode";
import {
	LanguageClient,
	LanguageClientOptions,
	ServerOptions,
	Trace,
	TransportKind,
} from "vscode-languageclient/node";

import path from "path";
let client: LanguageClient;

export function activate(context: ExtensionContext) {
	// The server is implemented in node
	let serverExe = "dotnet";
	const p = path.join(
		__dirname,
		"..",
		"..",
		"Server",
		"LSP",
		"bin",
		"Debug",
		"net8.0",
		"LSP.dll"
	);

	// If the extension is launched in debug mode then the debug server options are used
	// Otherwise the run options are used
	let serverOptions: ServerOptions = {
		run: {
			transport: TransportKind.stdio,
			command: serverExe,
			args: [p],
		},
		debug: {
			transport: TransportKind.stdio,
			command: serverExe,
			args: [p],
		},
	};

	// Options to control the language client
	let clientOptions: LanguageClientOptions = {
		// Register the server for plain text documents
		documentSelector: [
			{
				pattern: "**/*.eagle",
			},
		],
		synchronize: {
			// Synchronize the setting section 'languageServerExample' to the server
			configurationSection: "eagleLanguageServer",
			fileEvents: workspace.createFileSystemWatcher("**/*.eagle"),
		},
	};

	// Create the language client and start the client.
	client = new LanguageClient(
		"eagleLanguageServer",
		"Eagle Language Server",
		serverOptions,
		clientOptions
	);
	client.trace = Trace.Verbose;
	client.start();
	client.warn("Client started");
}

export function deactivate(): Thenable<void> | undefined {
	if (!client) {
		return undefined;
	}
	return client.stop();
}
