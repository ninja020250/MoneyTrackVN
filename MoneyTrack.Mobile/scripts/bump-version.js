// scripts/bump-version.js
const fs = require("fs");
const path = require("path");

const appJsonPath = path.join(__dirname, "..", "app.json");

try {
  const appJson = JSON.parse(fs.readFileSync(appJsonPath, "utf8"));

  if (!appJson.expo || !appJson.expo.version) {
    throw new Error("expo.version not found in app.json");
  }

  const currentVersion = appJson.expo.version;
  const parts = currentVersion.split(".").map(Number);

  if (parts.length !== 3 || parts.some(isNaN)) {
    throw new Error(`Invalid version format "${currentVersion}". Expected format: x.y.z`);
  }

  // Increment patch version
  parts[2] += 1;
  const newVersion = parts.join(".");

  appJson.expo.version = newVersion;

  fs.writeFileSync(appJsonPath, JSON.stringify(appJson, null, 2) + "\n", "utf8");

  console.log(`✅ Version bumped: ${currentVersion} → ${newVersion}`);
} catch (err) {
  console.error("❌ Failed to bump version:", err.message);
  process.exit(1);
}
