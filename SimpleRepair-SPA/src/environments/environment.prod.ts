// for TSH
const ip = window.location.hostname;
const apiUrl = "http://" + ip + ":1001/api/";
const imageUrl = "http://" + ip + ":1001/uploaded/";

// TSHt Test Area
// const ip = window.location.hostname;
// const apiUrl = "http://" + ip + ":2001/api/";
// const imageUrl = "http://" + ip + ":2001/uploaded/";

export const environment = {
  production: true,

  // CB
  // apiUrl: "http://10.9.0.48:1001/api/",
  // imageUrl: "http://10.9.0.48:1001/uploaded/",

  // SPC
  // apiUrl: 'http://10.10.0.23:1001/api/',
  // imageUrl: 'http://10.10.0.23:1001/uploaded/',

  // SHC
  // apiUrl: 'http://10.4.0.75:1001/api/',
  // imageUrl: 'http://10.4.0.75:1001/uploaded/',

  // Test Area
  // CBt
  // apiUrl: "http://10.9.0.48:2001/api/",
  // imageUrl: "http://10.9.0.48:2001/uploaded/",

  // SPCt
  // apiUrl: 'http://10.10.0.23:2001/api/',
  // imageUrl: 'http://10.10.0.23:2001/uploaded/',

  // SHCt
  // apiUrl: 'http://10.4.0.75:2001/api/',
  // imageUrl: 'http://10.4.0.75:2001/uploaded/',

  // TSH
  apiUrl: apiUrl,
  imageUrl: imageUrl,
};
