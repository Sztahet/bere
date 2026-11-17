const fs = require("fs");
const vm = require("vm");
const assert = require("assert");

class Node {
  constructor(tag, text = "") { this.tag = tag; this.textContent = text; this.children = []; this.attributes = {}; this.hidden = false; this.className = ""; this.classList = {add: (...names) => { this.className += " " + names.join(" "); }, toggle() {}}; }
  append(...nodes) { this.children.push(...nodes); }
  replaceChildren(...nodes) { this.children = nodes; }
  setAttribute(key, value) { this.attributes[key] = value; }
  removeAttribute(key) { delete this.attributes[key]; }
  addEventListener() {}
  querySelector() { return new Node("a"); }
  focus() {}
  scrollIntoView() {}
}
const nodes = new Map();
const document = {
  getElementById(id) { if (!nodes.has(id)) nodes.set(id, new Node("div")); return nodes.get(id); },
  createElement(tag) { return new Node(tag); },
  createTextNode(text) { return new Node("#text", text); },
};
const handlers = {};
const window = {addEventListener(name, fn) { handlers[name] = fn; }, scrollTo() {}};
const location = {hash: "#mixed"};
const storyArchive = [{id: "mixed", name: "Mixed", stories: [
  {id: "draft", title: "Draft", draft: true, chapters: []},
  {id: "approved", title: "Approved", chapters: []},
]}];
const storyRoster = [{id: "mixed", name: "Mixed", element: "Ogień", role: "Tank"}];
const context = {document, window, location, storyArchive, storyRoster};
vm.runInNewContext(fs.readFileSync("riftoryn-unity/Web/story-archive.js", "utf8"), context);
function badges(node) { return (node.className.includes("approval-badge") ? 1 : 0) + node.children.reduce((n, child) => n + badges(child), 0); }
const cards = nodes.get("collection").children[1].children;
assert.equal(badges(cards[0]), 1, "draft part must have a badge");
assert.equal(badges(cards[1]), 0, "approved part must have no badge");
location.hash = "#mixed/draft"; handlers.hashchange();
assert.equal(badges(nodes.get("reader-meta")), 1, "draft reader must have badge");
assert.equal(badges(nodes.get("parts-navigation").children[0]), 1, "draft navigation part must have badge");
assert.equal(badges(nodes.get("parts-navigation").children[1]), 0, "approved navigation part must have no badge");
location.hash = "#mixed/approved"; handlers.hashchange();
assert.equal(badges(nodes.get("reader-meta")), 0, "approved reader must have no badge");
console.log("Story approval badges: mixed statuses and navigation OK.");
