// Number utilities
export function formatCurrency(amount, currency = "USD") {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency,
  }).format(amount);
}

export function formatNumber(num, decimals = 0) {
  return Number(num.toFixed(decimals)).toLocaleString();
}

// String utilities
export function capitalize(str) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

export function truncate(str, length) {
  return str.length > length ? str.substring(0, length) + "..." : str;
}

// Object utilities
export function omit(obj, ...keys) {
  const result = { ...obj };
  keys.forEach((key) => delete result[key]);
  return result;
}

export function pick(obj, ...keys) {
  const result = {};
  keys.forEach((key) => {
    if (key in obj) result[key] = obj[key];
  });
  return result;
}

// Array utilities
export function unique(arr) {
  return Array.from(new Set(arr));
}

export function chunk(arr, size) {
  const chunks = [];
  for (let i = 0; i < arr.length; i += size) {
    chunks.push(arr.slice(i, i + size));
  }
  return chunks;
}
