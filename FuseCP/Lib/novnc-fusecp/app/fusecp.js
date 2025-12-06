/*
* FuseCP Utility functions for noVNC
* Copyright (c) 2024, FuseCP
* FuseCP is distributed under the Creative Commons Share-alike license
*/

import * as WebUtil from "./webutil.js";

export default function FCPUI(UI) {
	this.consoletype = WebUtil.getQueryVar('console');
	this.vmid = WebUtil.getQueryVar('vmid');
	this.vmname = WebUtil.getQueryVar('vmname');
	this.nodename = WebUtil.getQueryVar('node');
	this.resize = WebUtil.getQueryVar('resize');
	this.lastFBWidth = undefined;
	this.lastFBHeight = undefined;
	this.sizeUpdateTimer = undefined;
	this.UI = UI;

	var baseUrl = '/nodes/' + this.nodename;
	var url;
	var params = { websocket: 1 };
	var title;

	switch (this.consoletype) {
		case 'kvm':
			baseUrl += '/qemu/' + this.vmid;
			url = baseUrl + '/vncproxy';
			title = "VM " + this.vmid;
			if (this.vmname) {
				title += " ('" + this.vmname + "')";
			}
			break;
		case 'lxc':
			baseUrl += '/lxc/' + this.vmid;
			url = baseUrl + '/vncproxy';
			title = "CT " + this.vmid;
			if (this.vmname) {
				title += " ('" + this.vmname + "')";
			}
			break;
		case 'shell':
			url = baseUrl + '/vncshell';
			title = "node '" + this.nodename + "'";
			break;
		case 'upgrade':
			url = baseUrl + '/vncshell';
			params.upgrade = 1;
			title = 'System upgrade on node ' + this.nodename;
			break;
		default:
			//throw 'implement me';
			break;
	}

	if (this.resize == 'scale' &&
		(this.consoletype === 'lxc' || this.consoletype === 'shell')) {
		var size = this.getFBSize();
		params.width = size.width;
		params.height = size.height;
	}

	this.baseUrl = baseUrl;
	this.url = url;
	this.params = params;
	document.title = title;
};

FCPUI.prototype = {
	urlEncode: function (object) {
		var i, value, params = [];

		for (i in object) {
			if (object.hasOwnProperty(i)) {
				value = object[i];
				if (value === undefined) value = '';
				params.push(encodeURIComponent(i) + '=' + encodeURIComponent(String(value)));
			}
		}

		return params.join('&');
	},

	API2Request: function (reqOpts) {
		var me = this;

		reqOpts.method = reqOpts.method || 'GET';

		var xhr = new XMLHttpRequest();

		xhr.onload = function () {
			var scope = reqOpts.scope || this;
			var result;
			var errmsg;

			if (xhr.readyState === 4) {
				var ctype = xhr.getResponseHeader('Content-Type');
				if (xhr.status === 200) {
					if (ctype.match(/application\/json;/)) {
						result = JSON.parse(xhr.responseText);
					} else {
						errmsg = 'got unexpected content type ' + ctype;
					}
				} else {
					errmsg = 'Error ' + xhr.status + ': ' + xhr.statusText;
				}
			} else {
				errmsg = 'Connection error - server offline?';
			}

			if (errmsg !== undefined) {
				if (reqOpts.failure) {
					reqOpts.failure.call(scope, errmsg);
				}
			} else {
				if (reqOpts.success) {
					reqOpts.success.call(scope, result);
				}
			}
			if (reqOpts.callback) {
				reqOpts.callback.call(scope, errmsg === undefined);
			}
		}

		var data = me.urlEncode(reqOpts.params || {});

		if (reqOpts.method === 'GET') {
			xhr.open(reqOpts.method, "/api2/json" + reqOpts.url + '?' + data);
		} else {
			xhr.open(reqOpts.method, "/api2/json" + reqOpts.url);
		}
		xhr.setRequestHeader('Cache-Control', 'no-cache');
		if (reqOpts.method === 'POST' || reqOpts.method === 'PUT') {
			xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
			xhr.setRequestHeader('CSRFPreventionToken', PVE.CSRFPreventionToken);
			xhr.send(data);
		} else if (reqOpts.method === 'GET') {
			xhr.send();
		} else {
			throw "unknown method";
		}
	},

	pve_detect_migrated_vm: function () {
		var me = this;
		if (me.consoletype === 'kvm') {
			// try to detect migrated VM
			me.API2Request({
				url: '/cluster/resources',
				method: 'GET',
				success: function (result) {
					var list = result.data;
					list.every(function (item) {
						if (item.type === 'qemu' && item.vmid == me.vmid) {
							var url = "?" + me.urlEncode({
								console: me.consoletype,
								novnc: 1,
								vmid: me.vmid,
								vmname: me.vmname,
								node: item.node,
								resize: me.resize
							});
							location.href = url;
							return false; // break
						}
						return true;
					});
				}
			});
		} else if (me.consoletype === 'lxc') {
			// lxc restart migration can take a while,
			// so we need to find out if we are really migrating
			var migrating;
			var check = setInterval(function () {
				if (migrating === undefined ||
					migrating === true) {
					// check (again) if migrating
					me.UI.showStatus('Waiting for connection...', 'warning', 5000);
					me.API2Request({
						url: me.baseUrl + '/config',
						method: 'GET',
						success: function (result) {
							var lock = result.data.lock;
							if (lock == 'migrate') {
								migrating = true;
								me.UI.showStatus('Migration detected, waiting...', 'warning', 5000);
							} else {
								migrating = false;
							}
						},
						failure: function () {
							migrating = false;
						}
					});
				} else {
					// not migrating any more
					me.UI.showStatus('Connection resumed', 'warning');
					clearInterval(check);
					me.API2Request({
						url: '/cluster/resources',
						method: 'GET',
						success: function (result) {
							var list = result.data;
							list.every(function (item) {
								if (item.type === 'lxc' && item.vmid == me.vmid) {
									var url = "?" + me.urlEncode({
										console: me.consoletype,
										novnc: 1,
										vmid: me.vmid,
										vmname: me.vmname,
										node: item.node,
										resize: me.resize
									});
									location.href = url;
									return false; // break
								}
								return true;
							});
						}
					});
				}
			}, 5000);
		}

	},

	pve_vm_command: function (cmd, params, reload) {
		var me = this;
		var baseUrl;
		var confirmMsg = "";

		switch (cmd) {
			case "start":
				reload = 1;
			case "shutdown":
			case "stop":
			case "reset":
			case "suspend":
			case "resume":
				confirmMsg = "Do you really want to " + cmd + " VM/CT {0}?";
				break;
			case "reload":
				location.reload();
				break;
			default:
				throw "implement me " + cmd;
		}

		confirmMsg = confirmMsg.replace('{0}', me.vmid);

		if (confirmMsg !== "" && confirm(confirmMsg) !== true) {
			return;
		}

		me.UI.closePVECommandPanel();

		if (me.consoletype === 'kvm') {
			baseUrl = '/nodes/' + me.nodename + '/qemu/' + me.vmid;
		} else if (me.consoletype === 'lxc') {
			baseUrl = '/nodes/' + me.nodename + '/lxc/' + me.vmid;
		} else {
			throw "unknown VM type";
		}

		me.API2Request({
			params: params,
			url: baseUrl + "/status/" + cmd,
			method: 'POST',
			failure: function (msg) {
				if (cmd === 'start' && msg.match(/already running/) !== null) {
					// we wanted to start, but it was already running, so
					// reload anyway
					me.UI.showStatus("VM command '" + cmd + "' successful", 'normal');
					setTimeout(function () {
						location.reload();
					}, 1000);
				} else {
					me.UI.showStatus(msg, 'warning');
				}
			},
			success: function () {
				me.UI.showStatus("VM command '" + cmd + "' successful", 'normal');
				if (reload) {
					setTimeout(function () {
						location.reload();
					}, 1000);
				};
			}
		});
	},

	addPVEHandlers: function () {
		var me = this;
		document.getElementById('pve_commands_button')
			.addEventListener('click', me.UI.togglePVECommandPanel);

		// show/hide the buttons
		document.getElementById('noVNC_disconnect_button')
			.classList.add('noVNC_hidden');
		if (me.consoletype === 'kvm') {
			document.getElementById('noVNC_clipboard_button')
				.classList.add('noVNC_hidden');
		}

		if (me.consoletype === 'shell' || me.consoletype === 'upgrade') {
			document.getElementById('pve_commands_button')
				.classList.add('noVNC_hidden');
		}

		// add command logic
		var commandArray = [
			{ cmd: 'start', kvm: 1, lxc: 1 },
			{ cmd: 'stop', kvm: 1, lxc: 1 },
			{ cmd: 'shutdown', kvm: 1, lxc: 1 },
			{ cmd: 'suspend', kvm: 1 },
			{ cmd: 'resume', kvm: 1 },
			{ cmd: 'reset', kvm: 1 },
			{ cmd: 'reload', kvm: 1, lxc: 1, shell: 1 },
		];

		commandArray.forEach(function (item) {
			var el = document.getElementById('pve_command_' + item.cmd);
			if (!el) {
				return;
			}

			if (item[me.consoletype] === 1) {
				el.onclick = function () {
					me.pve_vm_command(item.cmd);
				};
			} else {
				el.classList.add('noVNC_hidden');
			}
		});
	},

	getFBSize: function () {
		var oh;
		var ow;

		if (window.innerHeight) {
			oh = window.innerHeight;
			ow = window.innerWidth;
		} else if (document.documentElement &&
			document.documentElement.clientHeight) {
			oh = document.documentElement.clientHeight;
			ow = document.documentElement.clientWidth;
		} else if (document.body) {
			oh = document.body.clientHeight;
			ow = document.body.clientWidth;
		} else {
			throw "can't get window size";
		}

		return { width: ow, height: oh };
	},

	pveStart: function (callback) {
		var me = this;
		me.API2Request({
			url: me.url,
			method: 'POST',
			params: me.params,
			success: function (result) {
				var wsparams = me.urlEncode({
					port: result.data.port,
					vncticket: result.data.ticket
				});

				document.getElementById('noVNC_password_input').value = result.data.ticket;
				me.UI.forceSetting('path', 'api2/json' + me.baseUrl + '/vncwebsocket' + "?" + wsparams);

				callback();
			},
			failure: function (msg) {
				me.UI.showStatus(msg, 'error');
			}
		});
	},

	updateFBSize: function (rfb, width, height) {
		var me = this;
		try {
			// Note: window size must be even number for firefox
			me.lastFBWidth = Math.floor((width + 1) / 2) * 2;
			me.lastFBHeight = Math.floor((height + 1) / 2) * 2;

			if (me.sizeUpdateTimer !== undefined) {
				clearInterval(me.sizeUpdateTimer);
			}

			var update_size = function () {
				var clip = me.UI.getSetting('view_clip');
				var resize = me.UI.getSetting('resize');
				var autoresize = me.UI.getSetting('autoresize');
				if (clip || resize === 'scale' || !autoresize) {
					return;
				}

				// we do not want to resize if we are in fullscreen
				if (document.fullscreenElement || // alternative standard method
					document.mozFullScreenElement || // currently working methods
					document.webkitFullscreenElement ||
					document.msFullscreenElement) {
					return;
				}

				var oldsize = me.getFBSize();
				var offsetw = me.lastFBWidth - oldsize.width;
				var offseth = me.lastFBHeight - oldsize.height;
				if (offsetw !== 0 || offseth !== 0) {
					//console.log("try resize by " + offsetw + " " + offseth);
					try {
						window.resizeBy(offsetw, offseth);
					} catch (e) {
						console.log('resizing did not work', e);
					}
				}
			};

			update_size();
			me.sizeUpdateTimer = setInterval(update_size, 1000);

		} catch (e) {
			console.log(e);
		}
	},

	updateSessionSize: function (e) {
		var rfb = e.detail.rfb;
		var width = e.detail.width;
		var height = e.detail.height;
		UI.FCP.updateFBSize(rfb, width, height);

		UI.applyResizeMode();
		UI.updateViewClip();
	},

};
